"use client";

import { Provider } from "react-redux";
import { ReactNode } from "react";
import { store } from "../store";

interface ClientProviderProps {
  children: ReactNode;
}

export const StoreProvider = ({ children }: ClientProviderProps) => {
  return <Provider store={store}>{children}</Provider>;
};
