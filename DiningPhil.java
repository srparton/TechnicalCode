import java.util.concurrent.Semaphore;
import java.lang.Thread;
import java.util.concurrent.ThreadLocalRandom;

// import MyChop.Chop;

public class DiningPhil extends Thread {
// public class DiningPhil {
    
    public int thread_id; // Used in constructor for DiningPhil
    //private static final int MAX_TIME = 10000; // May not be needed in java
    private static final int EAT = 2; //How many times should each philo eat
    private static final int DINERS_NUM = 5; //Number of philos
    private static Semaphore[] forks = new Semaphore[DINERS_NUM];// Array of semaphrores for forks.
    private static Chop myChop = new Chop(DINERS_NUM);

    public DiningPhil(int id) {
        this.thread_id = id;

    }

    public void run(){
        System.out.println("Diner " + thread_id + " is sitting down at the table.");
        int timesEaten = 0;

        while (timesEaten < EAT){
            int sleepTime = ThreadLocalRandom.current().nextInt(0,2000); //determine a random amount of time between 0 and 2 seconds
            System.out.println("Diner " + thread_id + " says they are thinking.");
            try {
                Thread.sleep(sleepTime);
            } catch (InterruptedException e) {
                e.printStackTrace();
            } //Diner "Thinks"
            System.out.println("Diner " + thread_id + " says they are hungry.");


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
        System.out.println("Diner " + thread_id + " is full, they are leaving the table.");
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
