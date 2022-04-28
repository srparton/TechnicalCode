using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class DiningPhil
{
    internal class Chop
    {
        private int N;
        private bool[] available;
        
        public Chop(int num)
        {
            N = num;
            this.available = new bool[N];

            for (int i = 0; i < N; i++)
            {
                this.available[i] = true; 
            }
        }

        public void get_LR(int me)
        {
            lock (this)
            {
                while (!available[me])
                {
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch (ThreadInterruptedException e) 
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }

                available[me] = false;
                Console.WriteLine("Diner " + me + " has left fork " + me);
                get_R((me + 1) % N);
            }
        }

        public void get_R(int me)
        {
            while (!available[me])
            {
                try
                {
                    Monitor.Wait(this);
                }
                catch (ThreadInterruptedException e) 
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            available[me] = false;
            if (me == 0)
            {
                Console.WriteLine("Diner " + me + " has right fork " + me);
            }

            else
                Console.WriteLine("Diner " + (me - 1) + " has right fork " + me);
        }

        public void release(int me)
        {
            lock (this)
            {
                available[me] = true;
                available[(me + 1) % N] = true;
                Console.WriteLine("Diner " + me + " set forks down");
                Monitor.PulseAll(this);
            }
        }

        
    }
    
    public int thread_id;
    private static int EAT = 2;
    private static int DINERS_NUM = 5;
    private static Chop myChop = new Chop(DINERS_NUM);

    public DiningPhil(int id) => this.thread_id = id;

    public void run()
    {
        Console.WriteLine("Diner " + thread_id + " is sitting down at the table.");
        int timesEaten = 0;

        while (timesEaten < EAT)
        {
            Random r = new Random();
            int sleepTime = r.Next(2000);
            Console.WriteLine("Diner " + thread_id + " says they are thinking.");
            try
            {
                Thread.Sleep(sleepTime);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            Console.WriteLine("Diner " + thread_id + " says they are hungry.");
            myChop.get_LR(thread_id);
            timesEaten++;
            sleepTime = r.Next(2000);
            try
            {
                Thread.Sleep(sleepTime);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            myChop.release(thread_id);
        }
        Console.WriteLine("Diner " + thread_id + " is full, they are leaving the table.");
    }

    public static void Main(String[] args)
    {
        DiningPhil[] philo = new DiningPhil[DINERS_NUM];
        Task[] dine = new Task[DINERS_NUM];

        for (int i = 0; i < DINERS_NUM; i++)
        {
            philo[i] = new DiningPhil(i);
            dine[i] = Task.Run(philo[i].run);
        }

        Task.WaitAll(dine);
    }
}
