// src/routes/blogRoutes.js
import express from 'express';
import BlogController from '../controllers/BlogController.js';
import authMiddleware from '../middlewares/authMiddleware.js';

const router = express.Router();

router.post('/', authMiddleware, BlogController.createBlog);
router.get('/', BlogController.getBlogs);
router.get('/:id', BlogController.getBlogById);
router.put('/:id', authMiddleware, BlogController.updateBlog);
router.delete('/:id', authMiddleware, BlogController.deleteBlog);

export default router;
