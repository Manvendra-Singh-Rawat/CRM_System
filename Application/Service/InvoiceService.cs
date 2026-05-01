using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ClientManagement.Application.Service
{
    public class InvoiceService : IInvoiceService
    {
        private CreateInvoiceDTO invoiceData = new CreateInvoiceDTO();

        public async Task<byte[]> CreateInvoice(CreateInvoiceDTO invoiceData)
        {
            this.invoiceData = invoiceData;

            byte[] invoiceBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginVertical(40);
                    page.MarginHorizontal(40);

                    page.Header().Height(240)
                    .Element(ComposerHeader);

                    page.Content()
                    .Element(ComposeContent);

                    page.Footer().Height(40)
                    .Element(ComposeFooter);
                });
            }).GeneratePdf();

            return invoiceBytes;
        }

        // HEADER
        void ComposerHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("DEVIL HUNTER TECH").FontSize(20).FontFamily("GENISO");
                        column.Item().Text("200X, Near Anor Londo Church,\nRed Grave City\n").FontSize(12);
                        column.Item().Text($"Invoice #{invoiceData.InvoiceId}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                        column.Item().Text(text =>
                        {
                            text.Span("Date: ");
                            text.Span(DateOnly.FromDateTime(DateTime.Now).ToString());
                        });
                    });

                    row.ConstantItem(100).AlignRight().Image("..\\CRM_System\\CompanyLogo\\Logo_Project.png");
                });

                col.Item().Row(row =>
                {
                    row.RelativeItem().PaddingVertical(6).Column(colx =>
                    {
                        colx.Item()
                        .PaddingTop(4).PaddingBottom(8)
                        .Background(Colors.LightBlue.Accent3)
                        .PaddingVertical(4).PaddingLeft(4)
                        .Text("BILLING INFORMATION");

                        colx.Item().PaddingLeft(4).PaddingTop(4)
                        .Text(text =>
                        {
                            text.Span("Name: ");
                            text.Span(invoiceData.ClientName);
                        });
                        colx.Item().PaddingLeft(4).PaddingTop(8)
                        .Text(text =>
                        {
                            text.Span("Phone: ");
                            text.Span(invoiceData.ClientPhone);
                        });
                        colx.Item().PaddingLeft(4).PaddingTop(8)
                        .Text(text =>
                        {
                            text.Span("Email: ");
                            text.Span(invoiceData.ClientEmail);
                        });
                    });
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item().Element(ComposeTable);
                column.Item().Element(ComposeTaxes);
                //column.Item().Element(ComposeCostInWord);
                column.Item().Element(ComposeNote);
            });
        }
        void ComposeTable(IContainer container)
        {
            container
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.LightBlue.Accent3)
                        .Element(CellStyle).Text("#")
                        .AlignCenter();

                        header.Cell().Background(Colors.LightBlue.Accent3)
                        .Element(CellStyle).PaddingLeft(4).Text("Project")
                        .AlignLeft();

                        header.Cell().Background(Colors.LightBlue.Accent3)
                        .Element(CellStyle).PaddingRight(4).Text("Price")
                        .AlignCenter();

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold())
                                .Border(1)
                                .BorderColor(Colors.LightBlue.Accent4).PaddingVertical(4);
                        }
                    });

                    {
                        table.Cell().Element(CellStyle)
                        .Text("1").AlignCenter();

                        table.Cell().Element(CellStyle).PaddingLeft(4)
                        .Text(invoiceData.ProjectName).AlignLeft();

                        table.Cell().Element(CellStyle).PaddingRight(4)
                        .Text($"{invoiceData.ProjectCost} ₹").AlignCenter();

                        static IContainer CellStyle(IContainer container)
                        {
                            return container
                                .BorderLeft(1).BorderRight(1).BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten1).PaddingVertical(4);
                        }
                    }
                });
        }

        void ComposeTaxes(IContainer container)
        {
            container
                .AlignMiddle().AlignRight().Column(column =>
                {
                    column.Item().MaxWidth(245).Row(row =>
                    {
                        row.RelativeItem().PaddingVertical(4).Text("SUB TOTAL");
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(4)
                        .Text(invoiceData.ProjectCost.ToString() + " ₹").AlignCenter();
                    });

                    column.Item().MaxWidth(245).Row(row =>
                    {
                        row.RelativeItem().PaddingVertical(4).Text("DISCOUNT");
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(4).Text("-").AlignCenter();
                    });

                    column.Item().MaxWidth(245).Row(row =>
                    {
                        row.RelativeItem().PaddingVertical(4).Text("TAX");
                        row.RelativeItem()
                        .Border(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(4)
                        .Text((invoiceData.ProjectCost * 18 / 100).ToString() + " ₹").AlignCenter();
                    });

                    column.Item().MaxWidth(245).Row(row =>
                    {
                        row.RelativeItem().PaddingVertical(4).Text("TOTAL");
                        row.RelativeItem()
                        .Border(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(4)
                        .Text((invoiceData.ProjectCost + (invoiceData.ProjectCost * 18 / 100)).ToString() + " ₹").AlignCenter();
                    });
                });
        }

        void ComposeCostInWord(IContainer container)
        {
            var cost = invoiceData.ProjectCost + (invoiceData.ProjectCost * 18 / 100);

            container
                .AlignMiddle().Padding(10).Column(column =>
                {
                    column.Item().Text(GetWords(cost)).FontSize(14).ExtraBold().FontColor(Colors.Red.Medium).AlignCenter();
                });
        }

        string GetWords(double cost)
        {
            int tempCost = (int)cost;

            int count = 0;

            while(tempCost > 0)
            {
                int digit = tempCost % 10;

            }
            return "One Thousand Two Hundred Eighty Rupees Only";
        }

        void ComposeNote(IContainer container)
        {
            container
                .Background(Colors.Grey.Lighten3)
                .AlignMiddle().Padding(10).Column(column =>
                {
                    column.Item()
                    .Text("* THIS IS A TEST INVOICE, DO NOT USE IT FOR ANY PURPOSE *")
                    .FontSize(12).Bold().FontColor(Colors.Red.Darken2).AlignCenter();
                });
        }

        void ComposeFooter(IContainer container) => 
            container.Height(40).AlignBottom().AlignCenter().Column(column =>
            {
                column.Item().Text("Page 1").Underline();
            });
    }
}
