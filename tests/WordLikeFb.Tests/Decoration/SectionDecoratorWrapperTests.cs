using System.Windows.Documents;
using WordLikeFb.Decoration;
using WordLikeFb.Decorators;
using WordLikeFb.Documents;

namespace WordLikeFb.Tests.Decoration
{
    public enum CreateTarget
    {
        Body,
        Section
    }

    public class SectionDecoratorWrapperTests
    {
        SectionDecoratorWrapper<SectionStartEndDecorator> CreateSut()
        {
            return new();
        }

        [WpfTheory]
        [InlineData(CreateTarget.Body)]
        [InlineData(CreateTarget.Section)]
        public void One_node_decorated(CreateTarget target)
        {
            var flow = new FlowDocument();
            Section sect = target switch 
            { 
                CreateTarget.Body => new Body(),
                _ => new Section()
            };

            flow.Blocks.Add(sect);

            var sut = CreateSut();

            sut.Wrap(flow.Blocks);

            Assert.IsType<SectionStartEndDecorator>(flow.Blocks.FirstBlock);
        }

        [WpfTheory]
        [InlineData(CreateTarget.Body, typeof(Body))]
        [InlineData(CreateTarget.Section, typeof(Section))]
        public void One_node_decorated_with_correct_target(CreateTarget target, Type targetType)
        {
            var flowDoc = new FlowDocument();
            Section sect = target switch
            {
                CreateTarget.Body => new Body(),
                _ => new Section()
            };

            flowDoc.Blocks.Add(sect);

            var sut = CreateSut();

            sut.Wrap(flowDoc.Blocks);

            var decorator = flowDoc.Blocks.FirstBlock as SectionStartEndDecorator;
            Assert.NotNull(decorator);
            Assert.Equal(targetType, decorator.DecorationTarget.GetType());
        }

        [WpfTheory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Many_node_decorated(int nodesCount)
        {
            var flowDoc = new FlowDocument();
            for (int i = 0; i < nodesCount; i++)
            {
                flowDoc.Blocks.Add(new Body());
            }
            var sut = CreateSut();
            sut.Wrap(flowDoc.Blocks);

            var decoratedNodesCount = flowDoc.Blocks.Where(b => b is SectionStartEndDecorator).Count();

            Assert.Equal(nodesCount, decoratedNodesCount);
        }

        [WpfTheory]
        [InlineData(2,2)]
        [InlineData(3,3)]
        [InlineData(4,4)]
        public void Many_nodes_with_many_childs_decorated(int parentCount, int childCount)
        {
            var flowDoc = new FlowDocument();
            for (int i = 0; i < parentCount; i++)
            {
                var body = new Body();
                for (int j = 0; j < childCount; j++)
                {
                    body.Blocks.Add(new Section());
                }
                flowDoc.Blocks.Add(body);
            }
            var sut = CreateSut();

            sut.Wrap(flowDoc.Blocks);

            var totalCount = 0;
            foreach (var block in flowDoc.Blocks)
            {
                if(block is SectionStartEndDecorator decoratedBody && decoratedBody.DecorationTarget is Body)
                {
                    totalCount++;
                    foreach (var childBlock in decoratedBody.DecorationTarget.Blocks)
                    {
                        if(childBlock is SectionStartEndDecorator decoratedSection && decoratedSection.DecorationTarget is Section)
                        {
                            totalCount++;
                        }
                    }
                }
            }

            Assert.Equal(parentCount + parentCount * childCount, totalCount);
        }
    }
}
