import { Dispatch, Action, Middleware, MiddlewareAPI } from "@reduxjs/toolkit";
import { clearNotifications } from "../redux/slices/bookSlice";
import { RootState } from "../redux/store";

export const clearNotificationsMiddleware: Middleware<
  Dispatch<Action>,
  RootState
> =
  (storeApi: MiddlewareAPI<Dispatch<Action>, RootState>) =>
  (next: Dispatch<Action>) =>
  (action: Action) => {
    next(action);

    if (
      action.type.includes("Success") ||
      action.type.includes("Error") ||
      action.type.includes("Warning")
    ) {
      setTimeout(() => {
        storeApi.dispatch(clearNotifications());
      }, 5000);
    }
  };
