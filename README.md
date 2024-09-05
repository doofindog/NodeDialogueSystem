


https://github.com/user-attachments/assets/b467c45b-cb2b-4a8e-a491-267ff98a3cef


# Introduction
The dialogue system I created is ingeniously crafted with a node-based architecture, making it highly adaptable and easy to manage. This system, built with Unity and C#, employs optimized reflection and serialization methods to handle conversations smoothly, showcasing a sophisticated blend of technology and user-friendly design.

# Key Steps in Implementation
1. Dialogue Database : Serves as a repository for conversations, ideally tailored for specific characters. This is where all dialogue data is stored and managed. By storing dialogues in a structured manner, the Dialogue Database ensures that conversations are easily accessible, modifiable, and reusable across different parts of your game. This centralized approach is particularly useful when dealing with complex narratives and multiple characters.
2. Node and Conversation Setup : Databases holds Conversation, Conversation holds dialogues. Dialogues, represented by nodes, are the most granular level in this hierarchy. Each node is an individual element of the conversation, containing the actual dialogue text, decisions Options and any parameters required for the specific dialogue event.
3. The Dialogue Editor Window : is a crucial component of the node-based dialogue system. It provides a visual interface for creating, editing, and managing dialogues without requiring prior coding experience. This window is designed to make the process intuitive and accessible for designers, writers, and anyone involved in developing game narratives. Additionally, it facilitates the integration of localization, allowing for dialogues to be easily translated and adapted for different languages.
3. Game Setup : One of the key advantages of the node-based dialogue system is its straightforward setup and seamless integration with character components. This ease of use ensures that developers can quickly attach and configure the dialogue system for any character in the game, facilitating a smooth workflow and consistent dialogue management across all game characters.

# Project Info
Time frame: 3 Weeks
Engine, Tools & Concepts: Unity Engine, Unity Editor Elements, Custom Serialization, Reflections and Graph
Language & Data Format : C#, JSON

#
