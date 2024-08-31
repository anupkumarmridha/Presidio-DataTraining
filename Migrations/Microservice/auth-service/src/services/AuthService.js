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
}

export default new AuthService();
