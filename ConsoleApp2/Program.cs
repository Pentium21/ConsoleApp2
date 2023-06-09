using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    public class MemoryManager
    {
        private const int MaxSegmentSize = 500; // Максимальный размер сегмента памяти

        private class MemoryBlock
        {
            public int Size { get; set; }
            public bool IsUsed { get; set; }
            public int Address { get; set; }
        }

        private readonly List<MemoryBlock> memoryBlocks;
        private int totalReservedMemory;

        public MemoryManager()
        {
            memoryBlocks = new List<MemoryBlock>();
            totalReservedMemory = 0;
        }

        public void AllocateMemory(int size)
        {
            if (size <= 0 || size > MaxSegmentSize - totalReservedMemory)
            {
                Console.WriteLine("Неудачное выделение памяти. Размер блока превышает доступное свободное место.");
                return;
            }

            int availableAddress = GetAvailableAddress();
            if (availableAddress == -1)
            {
                Console.WriteLine("Неудачное выделение памяти. Недостаточно свободного места.");
                return;
            }

            MemoryBlock block = new MemoryBlock
            {
                Size = size,
                IsUsed = true,
                Address = availableAddress
            };

            memoryBlocks.Add(block);
            totalReservedMemory += size;
            Console.WriteLine($"Выделена память блоком размером {size} на адресе {availableAddress}.");

            LogMemoryUsage();
        }

        public void FreeMemory(int address)
        {
            MemoryBlock block = memoryBlocks.Find(b => b.Address == address && b.IsUsed);
            if (block != null)
            {
                block.IsUsed = false;
                totalReservedMemory -= block.Size;
                Console.WriteLine($"Освобождена память блока размером {block.Size} на адресе {block.Address}.");

                LogMemoryUsage();

                OptimizeMemory();
            }
            else
            {
                Console.WriteLine($"Неудачное освобождение памяти. Блок на адресе {address} не найден или уже освобожден.");
            }
        }

        private int GetAvailableAddress()
        {
            int address = 0;
            foreach (MemoryBlock block in memoryBlocks)
            {
                if (!block.IsUsed)
                {
                    return block.Address;
                }
                address = block.Address + block.Size;
            }
            return (address < MaxSegmentSize) ? address : -1;
        }

        private void OptimizeMemory()
        {
            memoryBlocks.RemoveAll(block => !block.IsUsed);

            int currentAddress = 0;
            foreach (MemoryBlock block in memoryBlocks)
            {
                block.Address = currentAddress;
                currentAddress += block.Size;
            }
        }

        public void LogMemoryUsage()
        {
            Console.WriteLine("Текущее использование памяти:");
            foreach (MemoryBlock block in memoryBlocks)
            {
                Console.WriteLine($"Блок памяти размером {block.Size} на адресе {block.Address}. Статус: {(block.IsUsed ? "используется" : "не используется")}.");
            }

            Console.WriteLine($"Общий размер памяти, зарезервированный транслятором: {totalReservedMemory}.");
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MemoryManager memoryManager = new MemoryManager();

            // Запросить пять блоков памяти разного размера
            memoryManager.AllocateMemory(100);
            memoryManager.AllocateMemory(200);
            memoryManager.AllocateMemory(150);
            memoryManager.AllocateMemory(300);
            memoryManager.AllocateMemory(250);

            // Освободить память для 2 и 4 блока
            memoryManager.FreeMemory(200);
            memoryManager.FreeMemory(800);

            // Освободить память для 3 блока
            memoryManager.FreeMemory(300);

            // Запросить память чуть большего размера, чем можно выделить для одного сегмента
            memoryManager.AllocateMemory(1200);

            // Запросить три раза памяти размером в половину из предыдущего пункта
            memoryManager.AllocateMemory(600);
            memoryManager.AllocateMemory(600);
            memoryManager.AllocateMemory(600);

            // Вывести статистику использования памяти
            memoryManager.LogMemoryUsage();

            Console.ReadLine();
        }
    }
}






//    internal class Program
//    {
//        // Константы, задающие размер сегментов памяти
//        const int STACK_SEGMENT_SIZE = 3;
//        const int QUEUE_SEGMENT_SIZE = 3;
//        const int DEQUE_SEGMENT_SIZE = 3;
//        static void Main(string[] args)
//        {
//            // Создаем структуру для стека
//            Stack stack = new Stack(STACK_SEGMENT_SIZE);

//            // Создаем структуру для очереди
//            Queue queue = new Queue(QUEUE_SEGMENT_SIZE);

//            // Создаем структуру для дека
//            Deque deque = new Deque(DEQUE_SEGMENT_SIZE);

//            // Добавляем три элемента в стек
//            stack.Push(1);
//            stack.Push(2);
//            stack.Push(3);

//            // Выводим содержимое сегментов памяти стека
//            Console.WriteLine("Stack memory segments after pushing 3 elements:");
//            stack.PrintSegments();

//            // Выбираем и удаляем элемент из стека
//            int stackElement = stack.Pop();

//            // Выводим выбранный элемент и содержимое сегментов памяти стека
//            Console.WriteLine($"Selected element from stack: {stackElement}");
//            Console.WriteLine("Stack memory segments after popping 1 element:");
//            stack.PrintSegments();

//            // Добавляем три элемента в очередь
//            queue.Enqueue(1);
//            queue.Enqueue(2);
//            queue.Enqueue(3);

//            // Выводим содержимое сегментов памяти очереди
//            Console.WriteLine("Queue memory segments after enqueueing 3 elements:");
//            queue.PrintSegments();

//            // Выбираем и удаляем элемент из очереди
//            int queueElement = queue.Dequeue();

//            // Выводим выбранный элемент и содержимое сегментов памяти очереди
//            Console.WriteLine($"Selected element from queue: {queueElement}");
//            Console.WriteLine("Queue memory segments after dequeuing 1 element:");
//            queue.PrintSegments();

//            // Добавляем два элемента в начало дека и один в конец
//            deque.PushFront(1);
//            deque.PushFront(2);
//            deque.PushBack(3);

//            // Выводим содержимое сегментов памяти дека
//            Console.WriteLine("Deque memory segments after adding 3 elements:");
//            deque.PrintSegments();

//            // Выбираем и удаляем элемент из начала дека
//            int dequeFrontElement = deque.PopFront();

//            // Выводим выбранный элемент и содержимое сегментов памяти дека
//            Console.WriteLine($"Selected element from front of deque: {dequeFrontElement}");
//            Console.WriteLine("Deque memory segments after popping front element:");
//            deque.PrintSegments();

//            // Выбираем и удаляем элемент из конца дека
//            int dequeBackElement1 = deque.PopBack();

//            // Выводим выбранный элемент и содержимое сегментов памяти дека
//            Console.WriteLine($"Selected element from back of deque: {dequeBackElement1}");
//            Console.WriteLine("Deque memory segments after popping back element:");
//            deque.PrintSegments();
//            // Выбираем и удаляем элемент из начала дека
//            int dequeFrontElement2 = deque.PopFront();

//            // Выводим выбранный элемент и содержимое сегментов памяти дека
//            Console.WriteLine($"Selected element from front of deque: {dequeFrontElement2}");
//            Console.WriteLine("Deque memory segments after popping front element:");
//            deque.PrintSegments();

//            // Пытаемся выбрать и удалить элемент из пустого дека
//            try
//            {
//                int emptyDequeElement = deque.PopFront();
//                Console.WriteLine($"Selected element from front of empty deque: {emptyDequeElement}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("An error occurred: " + ex.Message);
//            }
//        }
//    }

//    // Класс, представляющий стек
//    class Stack
//    {
//        private int[] memory; // Память для хранения элементов стека
//        private int MaxSegmentSize; // Размер сегмента памяти
//        private int top; // Индекс верхнего элемента стека

//        public Stack(int MaxSegmentSize)
//        {
//            this.MaxSegmentSize = MaxSegmentSize;
//            memory = new int[MaxSegmentSize];
//            top = -1;
//        }

//        public void Push(int element)
//        {
//            if (top == MaxSegmentSize - 1)
//            {
//                Console.WriteLine("Stack overflow");
//                return;
//            }

//            top++;
//            memory[top] = element;
//        }

//        public int Pop()
//        {
//            if (top == -1)
//            {
//                Console.WriteLine("Stack underflow");
//                return -1;
//            }

//            int element = memory[top];
//            top--;
//            return element;
//        }

//        public void PrintSegments()
//        {
//            for (int i = 0; i <= top; i++)
//            {
//                Console.WriteLine($"Segment {i}: {memory[i]}");
//            }

//            for (int i = top + 1; i < MaxSegmentSize; i++)
//            {
//                Console.WriteLine($"Segment {i}: Not used");
//            }
//        }
//    }

//    // Класс, представляющий очередь
//    class Queue
//    {
//        private int[] memory; // Память для хранения элементов очереди
//        private int MaxSegmentSize; // Размер сегмента памяти
//        private int front; // Индекс начала очереди
//        private int rear; // Индекс конца очереди
//        private int count; // Количество элементов в очереди

//        public Queue(int MaxSegmentSize)
//        {
//            this.MaxSegmentSize = MaxSegmentSize;
//            memory = new int[MaxSegmentSize];
//            front = 0;
//            rear = -1;
//            count = 0;
//        }

//        public void Enqueue(int element)
//        {
//            if (count == MaxSegmentSize)
//            {
//                Console.WriteLine("Queue overflow");
//                return;
//            }

//            rear = (rear + 1) % MaxSegmentSize;
//            memory[rear] = element;
//            count++;
//        }

//        public int Dequeue()
//        {
//            if (count == 0)
//            {
//                Console.WriteLine("Queue underflow");
//                return -1;
//            }

//            int element = memory[front];
//            front = (front + 1) % MaxSegmentSize;
//            count--;
//            return element;
//        }

//        public void PrintSegments()
//        {
//            for (int i = front; i <= rear; i++)
//            {
//                Console.WriteLine($"Segment {i}: {memory[i]}");
//            }

//            for (int i = count; i < MaxSegmentSize; i++)
//            {
//                Console.WriteLine($"Segment {i}: Not used");
//            }
//        }
//    }

//    // Класс, представляющий дек
//    class Deque
//    {
//        private int[] memory; // Память для хранения элементов дека
//        private int MaxSegmentSize; // Размер сегмента памяти
//        private int front; // Индекс начала дека
//        private int rear; // Индекс конца дека
//        private int count; // Количество элементов в дека

//        public Deque(int MaxSegmentSize)
//        {
//            this.MaxSegmentSize = MaxSegmentSize;
//            memory = new int[MaxSegmentSize];
//            front = -1;
//            rear = -1;
//            count = 0;
//        }

//        public void PushFront(int element)
//        {
//            if (count == MaxSegmentSize)
//            {
//                Console.WriteLine("Deque overflow");
//                return;
//            }

//            if (front == -1)
//            {
//                front = 0;
//                rear = 0;
//            }
//            else
//            {
//                front = (front - 1 + MaxSegmentSize) % MaxSegmentSize;
//            }

//            memory[front] = element;
//            count++;
//        }

//        public void PushBack(int element)
//        {
//            if (count == MaxSegmentSize)
//            {
//                Console.WriteLine("Deque overflow");
//                return;
//            }

//            if (rear == -1)
//            {
//                front = 0;
//                rear = 0;
//            }
//            else
//            {
//                rear = (rear + 1) % MaxSegmentSize;
//            }

//            memory[rear] = element;
//            count++;
//        }

//        public int PopFront()
//        {
//            if (count == 0)
//            {
//                Console.WriteLine("Deque underflow");
//                return -1;
//            }

//            int element = memory[front];
//            if (front == rear)
//            {
//                front = -1;
//                rear = -1;
//            }
//            else
//            {
//                front = (front + 1) % MaxSegmentSize;
//            }

//            count--;
//            return element;
//        }

//        public int PopBack()
//        {
//            if (count == 0)
//            {
//                Console.WriteLine("Deque underflow");
//                return -1;
//            }

//            int element = memory[rear];
//            if (front == rear)
//            {
//                front = -1;
//                rear = -1;
//            }
//            else
//            {
//                rear = (rear - 1 + MaxSegmentSize) % MaxSegmentSize;
//            }

//            count--;
//            return element;
//        }

//        public void PrintSegments()
//        {
//            for (int i = 0; i < MaxSegmentSize; i++)
//            {
//                if (i >= front && i <= rear)
//                {
//                    Console.WriteLine($"Segment {i}: {memory[i]}");
//                }
//                else
//                {
//                    Console.WriteLine($"Segment {i}: Not used");
//                }
//            }
//        }
//    }
//}


