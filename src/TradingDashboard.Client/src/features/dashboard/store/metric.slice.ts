import { createSlice } from "@reduxjs/toolkit";
import type { WidgetDto } from "../types/dashboard.types";

export const metricSlice = createSlice({
  name: "metric",
  initialState: {
    metrics: [] as WidgetDto[],
  },
  reducers: {
    setMetrics(state, action) {
      state.metrics = state.metrics.some(
        (m) => m.widgetType === action.payload.widgetType,
      )
        ? state.metrics.map((m) =>
            m.widgetType === action.payload.widgetType ? action.payload : m,
          )
        : [...state.metrics, action.payload];
    },
  },
});
