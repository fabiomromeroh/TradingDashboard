// hooks/useCandlestickChart.ts
import { useEffect, useRef } from "react";
import {
  createChart,
  CandlestickSeries,
  LineSeries,
  HistogramSeries,
} from "lightweight-charts";
import type {
  IChartApi,
  ISeriesApi,
  CandlestickData,
  Time,
  HistogramData,
  LineData,
} from "lightweight-charts";

export interface Candle extends CandlestickData<Time> {
  volume?: number;
}

interface UseCandlestickChartOptions {
  data: Candle[];
  height?: number;
  movingAveragePeriod?: number; // e.g. 20
}

export function useCandlestickChart({
  data,
  height = 500,
  movingAveragePeriod = 20,
}: UseCandlestickChartOptions) {
  const containerRef = useRef<HTMLDivElement>(null);
  const legendRef = useRef<HTMLDivElement>(null);
  const chartRef = useRef<IChartApi | null>(null);
  const candleSeriesRef = useRef<ISeriesApi<"Candlestick"> | null>(null);
  const volumeSeriesRef = useRef<ISeriesApi<"Histogram"> | null>(null);
  const maSeriesRef = useRef<ISeriesApi<"Line"> | null>(null);

  useEffect(() => {
    if (!containerRef.current) return;

    const chart = createChart(containerRef.current, {
      height,
      layout: { background: { color: "#0f0f0f" }, textColor: "#d1d5db" },
      grid: {
        vertLines: { color: "#1f2937" },
        horzLines: { color: "#1f2937" },
      },
      timeScale: { timeVisible: true, secondsVisible: false },
      crosshair: { mode: 1 }, // Normal
    });

    // Pane 0: price
    const candleSeries = chart.addSeries(
      CandlestickSeries,
      {
        upColor: "#22c55e",
        downColor: "#ef4444",
        borderVisible: false,
        wickUpColor: "#22c55e",
        wickDownColor: "#ef4444",
      },
      0,
    );

    const maSeries = chart.addSeries(
      LineSeries,
      {
        color: "#60a5fa",
        lineWidth: 2,
        priceLineVisible: false,
      },
      0,
    );

    // Pane 1: volume, stacked below the price pane
    const volumeSeries = chart.addSeries(
      HistogramSeries,
      {
        priceFormat: { type: "volume" },
        color: "#374151",
      },
      1,
    );

    chart.panes()[1]?.setHeight(height * 0.2);

    chartRef.current = chart;
    candleSeriesRef.current = candleSeries;
    volumeSeriesRef.current = volumeSeries;
    maSeriesRef.current = maSeries;

    const resizeObserver = new ResizeObserver(() => {
      if (containerRef.current)
        chart.applyOptions({ width: containerRef.current.clientWidth });
    });
    resizeObserver.observe(containerRef.current);

    chart.subscribeCrosshairMove((param) => {
      if (!legendRef.current) return;
      const candle = param.seriesData.get(candleSeries) as
        | CandlestickData<Time>
        | undefined;
      if (!param.time || !candle) {
        legendRef.current.style.opacity = "0";
        return;
      }
      legendRef.current.style.opacity = "1";
      legendRef.current.innerHTML =
        `O <b>${candle.open.toFixed(2)}</b> ` +
        `H <b>${candle.high.toFixed(2)}</b> ` +
        `L <b>${candle.low.toFixed(2)}</b> ` +
        `C <b>${candle.close.toFixed(2)}</b>`;
    });

    return () => {
      resizeObserver.disconnect();
      chart.remove();
    };
  }, [height]);

  useEffect(() => {
    if (
      !candleSeriesRef.current ||
      !volumeSeriesRef.current ||
      !maSeriesRef.current
    )
      return;

    candleSeriesRef.current.setData(data as CandlestickData<Time>[]);

    const volumeData: HistogramData<Time>[] = data.map((c) => ({
      time: c.time,
      value: c.volume ?? 0,
      color: c.close >= c.open ? "#22c55e55" : "#ef444455",
    }));
    volumeSeriesRef.current.setData(volumeData);

    const maData: LineData<Time>[] = calculateSMA(data, movingAveragePeriod);
    maSeriesRef.current.setData(maData);

    chartRef.current?.timeScale().fitContent();
  }, [data, movingAveragePeriod]);

  return { containerRef, legendRef };
}

function calculateSMA(data: Candle[], period: number): LineData<Time>[] {
  const result: LineData<Time>[] = [];
  for (let i = period - 1; i < data.length; i++) {
    const slice = data.slice(i - period + 1, i + 1);
    const avg = slice.reduce((sum, c) => sum + c.close, 0) / period;
    result.push({ time: data[i].time, value: avg });
  }
  return result;
}
