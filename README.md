# Visual Pathfinder
Thank you for taking a look at my project! I developed this project because I have started to love algorithms and data structures and wanted to create something that would help me and others better visualize them.\
You can see the algorithms in action here: https://radumicea.github.io/Visual-Pathfinder/

## Algorithms
At the moment, this project contains the following search algorithms:
### Dijkstra's Algorithm
Dijkstra's algorithm is an algorithm for finding the shortest paths between nodes in a graph. It is one of the first searching algorithms and is at the basis of many algorithms.  You could consider it the "ancestor" of search algorithms.\
\
For a given node in a graph, this algorithm finds the shortest path between that node and every other node. If we stop the search once a given destination node has been reached, the shortest path from the starting node to the destination node can be constructed. For this project, this is the way I have implemented the search to be done.
### A* Search Algorithm
A* is a widely used, complete, optimal, and efficient graph traversal and searching algorithm. It can be seen as an extension of Dijkstra's algorithm.\
\
This algorithm estimates the distance from the starting node to any other node through the use of a __heuristic__ function that is specific to the graph at hand. By comparing the sum of the actual and estimated distance of a node to that of another, it can choose an optimal path and only expand the search in the direction of the destination node.

## Implementation details
### Dijkstra's Algorithm implementation
Because _A* can be seen as an extension of Dijkstra's algorithm_, I have chosen to implement Dijkstra's as a "restriction" of A*; that is an A* with a heuristic function that returns 0 for any input. That way A* will have no estimation, and will behave as Dijkstra's.
### Chosen graph structure
The nice thing about my implementation is that __any__ class that extends the Graph class (which in turn is generic and can have as nodes any class that implements the INodeInterface) is suitable to be searched on. However, for the sake of simplicity and visual representation, for this project I have opted to showcase the algorithms on a graph that takes the form of a 2D grid map.
### Use of multiple destination (interest) points
The two algorithms __guarantee__ the shortest path between the start node and the closest (as long as there are no obstacles) interest point. By marking a reached interest point as start and then starting the search again from said node, it can find the shortest path between any two nodes marked on the grid map. It does _not_ however guarantee the shortest path through all the interest points.
### Obstacles and A*
Because the heuristic function can not account for the obstacles and the distance associated with going around them, A* will often pass right by closer points, in its search for the point it believes is closest.
