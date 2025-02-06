"use client";
import { combineReducers } from "redux";
import bookSlice from "../slices/bookSlice";

export const rootReducer = combineReducers({
  book: bookSlice,
});
