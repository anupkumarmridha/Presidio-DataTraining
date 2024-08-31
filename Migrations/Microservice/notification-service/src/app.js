import express from 'express';
import dotenv from 'dotenv';
import connectDB from './config/db.js';
import notificationRoutes from './routes/notificationRoutes.js';

dotenv.config();

const app = express();
const PORT = process.env.PORT || 5002;

// Connect to the database
connectDB();

// Middleware
app.use(express.json());

// Routes
app.use('/api/notifications', notificationRoutes);

// Error handling middleware
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send('Something went wrong!');
});

// Start server
app.listen(PORT, () => console.log(`Notification service running on port ${PORT}`));
