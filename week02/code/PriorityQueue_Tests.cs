using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Add items with different priorities (Item1=5, Item2=3, Item3=7) then dequeue. Should remove Item3 (highest priority=7)
    // Expected Result: Dequeue returns "Item3" first, then "Item1", then "Item2"
    // Defect(s) Found: Dequeue() loop had incorrect bounds (index < _queue.Count - 1 instead of index < _queue.Count), missing _queue.RemoveAt() call to actually remove items, and used >= instead of > for priority comparison (breaking FIFO for equal priorities).
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        
        priorityQueue.Enqueue("Item1", 5);
        priorityQueue.Enqueue("Item2", 3);
        priorityQueue.Enqueue("Item3", 7);
        
        Assert.AreEqual("Item3", priorityQueue.Dequeue(), "Item with highest priority (7) should be removed first");
        Assert.AreEqual("Item1", priorityQueue.Dequeue(), "Next highest priority (5) should be removed");
        Assert.AreEqual("Item2", priorityQueue.Dequeue(), "Lowest priority (3) should be removed last");
    }

    [TestMethod]
    // Scenario: Add items with same highest priority (Item1=5, Item2=5, Item3=3). Should remove Item1 first (FIFO among equal priorities)
    // Expected Result: Dequeue returns "Item1", then "Item2", then "Item3"
    // Defect(s) Found: Dequeue() loop had incorrect bounds (index < _queue.Count - 1 instead of index < _queue.Count), missing _queue.RemoveAt() call to actually remove items, and used >= instead of > for priority comparison (breaking FIFO for equal priorities).
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        
        priorityQueue.Enqueue("Item1", 5);
        priorityQueue.Enqueue("Item2", 5);
        priorityQueue.Enqueue("Item3", 3);
        
        Assert.AreEqual("Item1", priorityQueue.Dequeue(), "When priorities are equal, first one added (FIFO) should be removed");
        Assert.AreEqual("Item2", priorityQueue.Dequeue(), "Next item with same priority should follow");
        Assert.AreEqual("Item3", priorityQueue.Dequeue(), "Lowest priority item should be last");
    }

    [TestMethod]
    // Scenario: Try to dequeue from an empty queue
    // Expected Result: InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: None - exception handling was correct.
    public void TestPriorityQueue_3()
    {
        var priorityQueue = new PriorityQueue();
        
        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown for empty queue.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                string.Format("Unexpected exception of type {0} caught: {1}",
                              e.GetType(), e.Message)
            );
        }
    }

    [TestMethod]
    // Scenario: Single item in queue with priority, dequeue it
    // Expected Result: Returns the single item correctly
    // Defect(s) Found: None - single item case passed.
    public void TestPriorityQueue_4()
    {
        var priorityQueue = new PriorityQueue();
        
        priorityQueue.Enqueue("OnlyItem", 10);
        
        Assert.AreEqual("OnlyItem", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Multiple items, add more after some dequeues (Item1=2, Item2=8, Item3=5, dequeue Item2, then add Item4=9, Item5=5)
    // Expected Result: Should dequeue Item2 (priority 8), then Item4 (priority 9), then Item5 (priority 5), then Item1 (priority 2), then Item3 (priority 5)
    // Defect(s) Found: Dequeue() loop had incorrect bounds (index < _queue.Count - 1 instead of index < _queue.Count), missing _queue.RemoveAt() call to actually remove items, and used >= instead of > for priority comparison (breaking FIFO for equal priorities).
    public void TestPriorityQueue_5()
    {
        var priorityQueue = new PriorityQueue();
        
        priorityQueue.Enqueue("Item1", 2);
        priorityQueue.Enqueue("Item2", 8);
        priorityQueue.Enqueue("Item3", 5);
        
        Assert.AreEqual("Item2", priorityQueue.Dequeue(), "Item with priority 8 should be removed first");
        
        priorityQueue.Enqueue("Item4", 9);
        priorityQueue.Enqueue("Item5", 5);
        
        Assert.AreEqual("Item4", priorityQueue.Dequeue(), "Item4 with priority 9 (highest) should be removed next");
        Assert.AreEqual("Item3", priorityQueue.Dequeue(), "Item3 with priority 5 (closest to front among priority 5) should be removed");
        Assert.AreEqual("Item5", priorityQueue.Dequeue(), "Item5 with priority 5 should follow");
        Assert.AreEqual("Item1", priorityQueue.Dequeue(), "Item1 with priority 2 should be removed last");
    }
}