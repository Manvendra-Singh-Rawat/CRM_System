using ClientManagement.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;

namespace ClientManagement.Application.Service
{
    public class InvoiceService : IInvoiceService
    {
        public Task CreateInvoice()
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header().Height(100).Background(Colors.Blue.Lighten4).Element(ComposerHeader);
                    page.Content().Background(Colors.Red.Lighten4).Element(ComposeContent);
                    page.Footer().Height(40).Background(Colors.Grey.Lighten1).Element(ComposeFooter);
                });
            }).GeneratePdfAndShow();

            return Task.CompletedTask;
        }

        void ComposerHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item()
                    .Text("Invoice #3823492").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span("19.03.2026");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Due Date: ").SemiBold();
                        text.Span("31.03.2026");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("CHECK").Bold();
                    });
                });

                row.ConstantItem(100).AlignRight().Height(70)
                .Image("C:\\Users\\manve\\Downloads\\Logo_Project.png");
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item().Element(ComposeTable);

                column.Item().AlignRight().Text("Grand Total: 3,999.00$");

                column.Item().Element(ComposeComments);
            });
        }
        void ComposeTable(IContainer container)
        {
            container
                .Background(Colors.BlueGrey.Lighten1)
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Red.Accent4).Element(CellStyle).Text("#").AlignCenter();
                        header.Cell().Background(Colors.Blue.Accent4).Element(CellStyle).PaddingLeft(4).Text("Product").AlignLeft();
                        header.Cell().Background(Colors.Green.Accent4).Element(CellStyle).PaddingRight(4).Text("Unit Price").AlignRight();
                        header.Cell().Background(Colors.Yellow.Accent4).Element(CellStyle).PaddingRight(4).Text("Quantity").AlignRight();
                        header.Cell().Background(Colors.DeepOrange.Accent4).Element(CellStyle).PaddingRight(4).Text("Total").AlignRight();

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold())
                                .PaddingVertical(5).BorderBottom(1)
                                .BorderColor(Colors.Black);
                        }
                    });

                    {
                        table.Cell().Element(CellStyle).Text("1").AlignCenter();
                        table.Cell().Element(CellStyle).PaddingLeft(4).Text("Product 1").AlignLeft();
                        table.Cell().Element(CellStyle).PaddingRight(4).Text("$3,999.0").AlignRight();
                        table.Cell().Element(CellStyle).PaddingRight(4).Text("1").AlignRight();
                        table.Cell().Element(CellStyle).PaddingRight(4).Text("$3,999.0").AlignRight();

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.PaddingVertical(5)
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    }
                });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(6);
                column.Item().Text("Comments").FontSize(14);
                column.Item().Text("lashdfpipuahpipvusrufaspu fnisurng iseurng oiserung egr").FontSize(12);
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.Height(40).AlignMiddle().AlignCenter().Column(column =>
            {
                column.Item().Text("Page 1");
            });
        }
    }
}
