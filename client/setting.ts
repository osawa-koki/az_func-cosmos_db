import Env from './next.config.js';
const isProd = process.env.NODE_ENV === 'production';

const setting = {
  isProd,
  basePath: Env.basePath,
  apiPath: isProd ? '' : 'http://localhost:8000/api',
  title: '🐙 Az Func × Cosmos DB 🐙',
  smallWaitingTime: 100,
  waitingTime: 3000,
};

export default setting;
