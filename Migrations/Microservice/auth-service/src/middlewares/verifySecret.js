import dotenv from 'dotenv';

dotenv.config();

export function verifyInternalServiceSecret(req, res, next) {
  const serviceSecret = req.headers['x-service-secret'];
  
  if (!serviceSecret || serviceSecret !== process.env.AUTH_SERVICE_SECRET) {
    return res.status(403).json({ message: 'Forbidden: Invalid or missing service secret' });
  }

  next();
}
