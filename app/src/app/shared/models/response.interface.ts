export interface ApiResponse<T = any> {
  httpCode: number;         // status code (like 200, 404)
  message?: string;         // any message from server
  data?: T;                 // generic payload
  status?: boolean;         // derived - optional
  isValidated?: boolean;    // derived - optional
}