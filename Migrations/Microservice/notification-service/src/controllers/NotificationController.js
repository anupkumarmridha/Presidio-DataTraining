// src/controllers/NotificationController.js
import NotificationService from '../services/NotificationService.js';

class NotificationController {
  async sendNotification(req, res) {
    try {
      const { userId, message } = req.body;
      const notification = await NotificationService.sendNotification(userId, message);
      res.status(201).json(notification);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async getNotifications(req, res) {
    try {
      const notifications = await NotificationService.getNotifications(req.user);
      res.status(200).json(notifications);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async markAsRead(req, res) {
    try {
      const notification = await NotificationService.markAsRead(req.params.id);
      res.status(200).json(notification);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }
}

export default new NotificationController();
