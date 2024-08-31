// src/controllers/BlogController.js
import BlogService from '../services/BlogService.js';

class BlogController {
  async createBlog(req, res) {
    try {
      const { title, content } = req.body;
      const author = req.user;
      const blog = await BlogService.createBlog(title, content, author);
      res.status(201).json(blog);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async getBlogs(req, res) {
    try {
      const blogs = await BlogService.getBlogs();
      res.status(200).json(blogs);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async getBlogById(req, res) {
    try {
      const blog = await BlogService.getBlogById(req.params.id);
      res.status(200).json(blog);
    } catch (error) {
      res.status(404).json({ message: error.message });
    }
  }

  async updateBlog(req, res) {
    try {
      const { title, content } = req.body;
      const author = req.user;
      const blog = await BlogService.updateBlog(req.params.id, title, content, author);
      res.status(200).json(blog);
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async deleteBlog(req, res) {
    try {
      const author = req.user;
      await BlogService.deleteBlog(req.params.id, author);
      res.status(200).json({ message: 'Blog deleted successfully' });
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }
}

export default new BlogController();
