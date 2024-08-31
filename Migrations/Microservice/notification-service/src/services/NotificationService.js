// src/services/NotificationService.js
import Notification from '../models/Notification.js';

class NotificationService {
  async sendNotification(userId, message) {
    const notification = new Notification({ userId, message });
    await notification.save();
    return notification;
  }

  async getNotifications(userId) {
    return Notification.find({ userId }).sort({ createdAt: -1 });
  }

  async markAsRead(notificationId) {
    const notification = await Notification.findByIdAndUpdate(
      notificationId,
      { read: true },
      { new: true }
    );
    if (!notification) {
      throw new Error('Notification not found');
    }
    return notification;
  }
}

export default new NotificationService();
