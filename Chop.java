// package MyChop;
// import java.lang.Thread;


public final class Chop{
    private int N ;
    private boolean available [] ;

    Chop (int N) { 
        this.N = N ;
        this.available = new boolean[N]; 
        for (int i =0 ; i < N ; i++) {
            available[i] = true ; // non allocated stick }
        }
    }
    public synchronized void get_LR (int me) {
        while(!available[me]){
            try { 
                wait(); 
            } catch (InterruptedException e) {} 
        }
        available[me] = false ; 
        System.out.println("Diner "+me+" has left fork "+me);
        // score = 1; // left stick allocated
        // don’t release mutual exclusion lock and immediately requests second stick while ( !available [(me + 1)% N]) {
        get_R((me+1)%N);
        // while(!available[(me+1)%N]){
        //     try { 
        //         wait(); 
        //     } catch (InterruptedException e) {} 
        // }
        // available [(me + 1)% N] = false ; 
        // score = 2; // both sticks allocated }
    }

    public synchronized void get_R (int me) {
        // int score = 0 ; // pseudo program counter used for transcripting the Java code in Ada while ( !available [me]) {
            
            while(!available[me]){
            try { 
                wait(); 
            } catch (InterruptedException e) {} 
        }
        available[me] = false ; 
        if(me == 0){
            System.out.println("Diner 4 has right fork "+me);
        } else
            System.out.println("Diner "+(me-1)+" has right fork "+me);
        // System.out.println("Diner "+thread_id+" has right fork "+me);

        // score = 1; // left stick allocated
    }

    public synchronized void release (int me) {
        available [me] = true ; 
        available [(me + 1)% N] = true; 
        System.out.println("Diner "+me+" set forks down");
        notifyAll();
    }
    
    // public void run() {
    //     try {
    //         get_LR(thread_id);
    //     } catch (Exception e) {
    //         e.printStackTrace();
    //     }
    // }
}