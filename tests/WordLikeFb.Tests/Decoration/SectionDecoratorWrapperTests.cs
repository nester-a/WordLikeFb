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
        SectionStartEndDecoratorWrapper CreateSut()
        {
            return new() ;
        }

        [WpfTheory]
        [InlineData(CreateTarget.Body)]
        [InlineData(CreateTarget.Section)]
        public void One_node_wrapped(CreateTarget target)
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
        public void One_node_wrapped_with_correct_target(CreateTarget target, Type targetType)
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
        public void Many_node_wrapped(int nodesCount)
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
        public void Many_nodes_with_many_childs_wrapped(int parentCount, int childCount)
        {
            var flowDoc = new FlowDocument();
            for (int i = 0; i < parentCount; i++)
            {
                var body = new Body();
                flowDoc.Blocks.Add(body);
                for (int j = 0; j < childCount; j++)
                {
                    body.Blocks.Add(new Section());
                }
            }
            var sut = CreateSut();

            sut.Wrap(flowDoc.Blocks);

            var wrapped = flowDoc.Blocks.All(block =>  block is SectionStartEndDecorator decoratedBody && 
                                                       decoratedBody.DecorationTarget is Body body &&
                                                       body.Parent is SectionStartEndDecorator &&
                                                       body.Blocks.All(childBlock => childBlock is SectionStartEndDecorator decoratedSection && 
                                                                                     decoratedSection.DecorationTarget is Section section &&
                                                                                     section.Parent is SectionStartEndDecorator));
            
            Assert.True(wrapped);
        }

        [WpfTheory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void Many_nodes_with_many_childs_unwrapped(int parentCount, int childCount)
        {
            var flowDoc = new FlowDocument();
            for (int i = 0; i < parentCount; i++)
            {
                var body = new SectionStartEndDecorator(new Body());
                flowDoc.Blocks.Add(body);
                for (int j = 0; j < childCount; j++)
                {
                    body.Blocks.Add(new SectionStartEndDecorator(new Section()));
                }
            }
            var sut = CreateSut();

            sut.Unwrap(flowDoc.Blocks);

            var unwrapped = flowDoc.Blocks.All(block => block is Body body &&
                                                        body.Parent is FlowDocument &&
                                                        body.Blocks.All(childBlock => childBlock is Section section &&
                                                                                      section.Parent is Body));

            Assert.True(unwrapped);
        }
    }
}
