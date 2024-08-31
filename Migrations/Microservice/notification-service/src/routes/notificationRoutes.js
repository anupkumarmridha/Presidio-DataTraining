// src/routes/notificationRoutes.js
import express from 'express';
import NotificationController from '../controllers/NotificationController.js';
import authMiddleware from '../middlewares/authMiddleware.js';

const router = express.Router();

router.post('/', authMiddleware, NotificationController.sendNotification);
router.get('/', authMiddleware, NotificationController.getNotifications);
router.patch('/:id', authMiddleware, NotificationController.markAsRead);

export default router;
