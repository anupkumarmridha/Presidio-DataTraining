import AuthService from '../services/AuthService.js';

class AuthController {
  async register(req, res) {
    try {
      const { username, email, password } = req.body;
      const { user, token } = await AuthService.register(username, email, password);
      res.status(201).json({ user, token });
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }

  async login(req, res) {
    try {
      const { email, password } = req.body;
      const { user, token } = await AuthService.login(email, password);
      res.status(200).json({ user, token });
    } catch (error) {
      res.status(400).json({ message: error.message });
    }
  }
}

export default new AuthController();
