import User from '../models/User.js';
import { generateToken } from '../utils/jwtUtils.js';

class AuthService {
  async register(username, email, password) {
    const existingUser = await User.findOne({ email });
    if (existingUser) {
      throw new Error('User already exists');
    }

    const user = new User({ username, email, password });
    await user.save();
    const token = generateToken(user);
    return { user, token };
  }

  async login(email, password) {
    const user = await User.findOne({ email });
    if (!user) {
      throw new Error('Invalid credentials');
    }

    const isMatch = await user.comparePassword(password);
    if (!isMatch) {
      throw new Error('Invalid credentials');
    }

    const token = generateToken(user);
    return { user, token };
  }

  //get user details by ID
  async getUserById(userId) {
    try {
      const userIdStr = userId.toString();
      const user = await User.findById(userIdStr).select('-password');
      
      if (!user) {
        throw new Error('User not found');
      }
      
      return user;
    } catch (error) {
      console.error('Error in getUserById:', error);
      throw error;
    }
  }
}

export default new AuthService();
