using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class DiningPhil {
// public class DiningPhil {
    
    public int thread_id; // Used in constructor for DiningPhil
    private static sealed int EAT = 2; //How many times should each philo eat
    private static sealed int DINERS_NUM = 5; //Number of philos
    private static Chop myChop = new Chop(DINERS_NUM);

    public DiningPhil(int id) {
        this.thread_id = id;
    }

    public void run(){
        Console.WriteLine("Diner " + thread_id + " is sitting down at the table.");
        int timesEaten = 0;

        while (timesEaten < EAT){
            int sleepTime = ThreadLocalRandom.current().nextInt(0,2000); //determine a random amount of time between 0 and 2 seconds
            Console.WriteLine("Diner " + thread_id + " says they are thinking.");
            try {
                Thread.sleep(sleepTime);
            } catch (InterruptedException e) {
                e.printStackTrace();
            } //Diner "Thinks"
            Console.WriteLine("Diner " + thread_id + " says they are hungry.");


            myChop.get_LR(thread_id);
            timesEaten++;

            sleepTime = ThreadLocalRandom.current().nextInt(0,2000);
            try {
                Thread.sleep(sleepTime);
            } catch (InterruptedException e) {
                e.printStackTrace();
            } //Diner "eats" for rand time between 0 and 2 seconds

            myChop.release(thread_id);
        }
        Console.WriteLine("Diner " + thread_id + " is full, they are leaving the table.");
    }

    public static void main(String[] args) {

        DiningPhil[] philo = new DiningPhil[DINERS_NUM];

        // Initiallizing diners array with threads
        for (int i = 0; i < DINERS_NUM; i++) {
            philo[i] = new DiningPhil(i);
            philo[i].start();
        }

        for (int i = 0; i < philo.length; i++) {
            try {
                philo[i].join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}