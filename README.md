# Project: Mobile Expense Tracker App

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Live Demo](#live-demo)
- [Technologies Used](#technologies-used)
- [Explore the Code](#explore-code)

---

<h2 id="overview">Overview</h2>

This project was my first real dive into mobile development, and I learnt how to create a full app from the ground up. I launched Expense Tracker to see how we can help people, especially young professionals and students, take control of their finances as they start budgeting for the first time.

I used .NET MAUI to build it as a cross-platform app, which meant I got to learn a lot about how mobile apps work across Android, iOS, and even desktop environments. The process taught me about structuring an app using MVVM architecture, designing clean user interfaces, and thinking through how people actually interact with financial tools on a daily basis.

What started as a design idea turned into a real app with working screens, logic behind the buttons, and a simple, user-friendly layout. This project helped me grow my skills in both development and UX, and gave me a better appreciation of how much thought goes into making an app feel easy to use.

---

<h2 id="features">Features</h2>

The Expense Tracker app includes a range of features designed to make managing personal finances simple, visual, and accessible:

- **Quick Transaction Logging**:<br>Users can easily add income and expenses with just a few taps. There’s also speech-to-text support, making it faster to record transactions on the go without needing to type.
- **Receipt Capture & Storage**:<br>Transactions can contain a picture of a receipt, which can be uploaded from the gallery or captured with the device's camera. This is useful for keeping records all in one place.
- **Custom Budget Setup**:<br>Users can create and adjust budgets that fit their personal financial goals. The budgeting tool is flexible, so it adapts as their financial situation changes.
- **Visual Insights**:<br>Financial data is shown through clear, easy-to-understand charts and graphs. These visuals help users quickly spot trends in their spending habits and track progress toward their goals.
- **Reminder System for Upcoming Payments**:<br>The app notifies users about upcoming bills or when they’re close to exceeding their set budget. This helps avoid late payments and encourages smarter money habits.
- **Accessible Design Choices**:<br>The app uses high-contrast colors, clean layouts, and intuitive navigation. It also includes features like speech input and thoughtful visual tagging (e.g., green for income, red for expenses) to support users with different needs.
- **Swipe Gestures for Editing and Navigation**:<br>Users can swipe on transactions to edit or delete them, or move between income and expense views. This keeps interactions fluid and modern.

---

<h2 id="live-demo">Live Demo</h2>

https://github.com/user-attachments/assets/d5643643-ce69-4088-8788-f8623f110dbc

---

<h2 id="technologies-used">Technologies Used</h2>

The app was developed using a range of tools and frameworks aimed at creating a smooth cross-platform mobile experience:

- **.NET MAUI** – The main framework used to build the app for Android, iOS, and other platforms from a single codebase.
- **C#** – Primary programming language used for backend logic and app functionality.
- **XAML** – Used to design and structure the UI pages and layout components.
- **Visual Studio** – Main development environment used for coding, testing, and debugging.
- **MVVM Architecture** – For clean separation of concerns, with ViewModels handling logic and data binding to views.
- **Figma** – Used for creating wireframes and designing the app’s user interface before development.
- **Microsoft Cognitive Services Speech SDK** – Used for speech-to-text functionality, enabling users to enter transaction descriptions using voice input.
- **MAUI Essentials: Camera & Media Picker APIs** – Used to capture or upload receipt images and attach them to transactions.
- **Plugin.LocalNotification** – Enables local notifications for budget alerts and reminders.
- **Permissions Plugin** – Handles runtime permissions for microphone, camera, and storage access.
- **Newtonsoft.Json** – For serialising and storing transaction data locally.

---

<h2 id="explore-code">Explore the Code</h2>

This app was built using .NET MAUI, C#, and XAML with MVVM architecture. While it’s not actively maintained, you’re welcome to browse the source code or fork it for your own learning and experimentation.
