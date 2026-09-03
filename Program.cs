using System;



public class Program
{
    public class Node<T> where T : struct
    {
        private T value;

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }
        
        public Node<T>? nodeleft {get; set;}

        public Node<T>? noderight {get; set;}

        public Node( T value, Node<T>? nodeleft = null, Node<T>? noderight = null)
        {
            this.nodeleft = nodeleft;
            this.noderight = noderight;
            Value = value;
        }
        public void SearchNodes(Node<T>? node)
        {
        
            if (node == null)
            {
                return;
            }
            Console.Write($"{node.Value} ");
            SearchNodes(node.nodeleft);
            SearchNodes(node.noderight);
        }

        public void SearchNodes1(Node<T>? node)
        {
            if (node == null)
            {
                return;
            }
            SearchNodes1(node.nodeleft);
            Console.Write(node.value + " ");
            SearchNodes1(node.noderight);

        }
        public void SearchNodes2(Node<T>? node)
        {
            if ( node == null )
            {
                return;
            }
            SearchNodes2(node.noderight);
            Console.Write(node.value + " ");
            SearchNodes2(node.nodeleft);
        }
        
    }
    public static void Main()
    {
        Node<int> node7 = new Node<int>(30);
        Node<int> node6 = new Node<int>(15);
        Node<int> node5 = new Node<int>(8);
        Node<int> node4 = new Node<int>(6);
        Node<int> node3 = new Node<int>(20, node6, node7);
        Node<int> node2 = new Node<int>(5, node4, node5);
        Node<int> node1 = new Node<int>(10, node2, node3);

        node1.SearchNodes(node1); // лево корень право
        node1.SearchNodes2(node1); // право корень лево
        node1.SearchNodes1(node1); // лево корень право
        
        
            
    }
}

    


