import Blog from '../models/Blog.js';

class BlogService {
  async createBlog(title, content, author) {
    const blog = new Blog({ title, content, author });
    await blog.save();
    return blog;
  }

  async getBlogs() {
    return Blog.find().populate('author', 'username email');
  }

  async getBlogById(blogId) {
    const blog = await Blog.findById(blogId).populate('author', 'username email');
    if (!blog) {
      throw new Error('Blog not found');
    }
    return blog;
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
