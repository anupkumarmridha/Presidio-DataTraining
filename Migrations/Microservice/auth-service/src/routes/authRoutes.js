import express from 'express';
import AuthController from '../controllers/AuthController.js';
import { verifyInternalServiceSecret } from '../middlewares/verifySecret.js';

const router = express.Router();

router.post('/register', AuthController.register);
router.post('/login', AuthController.login);
router.get('/users/:id', verifyInternalServiceSecret, AuthController.getUserById);

export default router;
