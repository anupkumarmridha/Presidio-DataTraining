import Blog from '../models/Blog.js';
import axios from 'axios';
import dotenv from 'dotenv';

dotenv.config();

class BlogService {
  constructor() {
    this.authServiceUrl = process.env.AUTH_SERVICE_URL;
    this.authServiceSecret = process.env.AUTH_SERVICE_SECRET;
  }

  async createBlog(title, content, author) {
    const blog = new Blog({ title, content, author });
    await blog.save();
    return blog;
  }

  async getBlogs() {
    // Fetch blogs from the database
    const blogs = await Blog.find();

    // Fetch user details for each blog
    const blogsWithUserDetails = await Promise.all(
      blogs.map(async (blog) => {
        const user = await this.getUserDetails(blog.author);
        return {
          ...blog.toObject(),
          author: user,
        };
      })
    );

    return blogsWithUserDetails;
  }

  async getBlogById(blogId) {
    const blog = await Blog.findById(blogId);
    if (!blog) {
      throw new Error('Blog not found');
    }

    const user = await this.getUserDetails(blog.author);
    return {
      ...blog.toObject(),
      author: user,
    };
  }

  // Helper method to fetch user details from Auth Service
  async getUserDetails(userId) {

    // Convert userId to a string if it's an ObjectId
    const userIdStr = userId.toString();

    try {
      const response = await axios.get(`${this.authServiceUrl}/api/auth/users/${userIdStr}`, {
        headers: {
          'x-service-secret': this.authServiceSecret, // Include the secret in the request header
        },
      });
      return response.data;
    } catch (error) {
      if (error.response && error.response.status === 404) {
        throw new Error('User not found');
      } else {
        console.error('Error fetching user details:', error.message);
        throw new Error('Failed to fetch user details'); // General error message for other errors
      }
    }
  }

  async updateBlog(blogId, title, content, author) {
    const blog = await Blog.findOneAndUpdate(
      { _id: blogId, author },
      { title, content, updatedAt: Date.now() },
      { new: true }
    );
    if (!blog) {
      throw new Error('Blog not found or user not authorized');
    }
    return blog;
  }

  async deleteBlog(blogId, author) {
    const blog = await Blog.findOneAndDelete({ _id: blogId, author });
    if (!blog) {
      throw new Error('Blog not found or user not authorized');
    }
    return blog;
  }
}

export default new BlogService();
