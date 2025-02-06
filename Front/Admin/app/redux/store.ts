import { configureStore, Middleware } from "@reduxjs/toolkit";
import { rootReducer } from "./reducers/index";
import { clearNotificationsMiddleware } from "../middlewares/clearNotificationsMiddleware";

export type RootState = ReturnType<typeof rootReducer>;

const clearNotifications: Middleware = clearNotificationsMiddleware;

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(clearNotifications),
});

export type AppDispatch = typeof store.dispatch;
