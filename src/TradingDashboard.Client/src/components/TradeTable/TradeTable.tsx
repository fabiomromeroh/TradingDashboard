import { useEffect, useState } from "react"
import api from "../../services/api";

interface Trade {
  symbol: string;
  entryPrice: number;
  quantity: number;
  direction: string;
  status: string;
}

export function TradeTable() {

  const [trades, setTrades] = useState<Trade[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
      setLoading(true);
      api.get('/trades').then(response => {
            console.log('API Response:', response);
            setTrades(response.data || response || []);
            setError(null);
      }).catch(error => {
        console.error('Error fetching trades:', error);
        setError(error.message || 'Failed to fetch trades');
        setTrades([]);
      }).finally(() => {
        setLoading(false);
      });

  }, [])

  if (loading) {
    return <div><h1>Trade Table</h1><p>Loading trades...</p></div>
  }

  if (error) {
    return <div><h1>Trade Table</h1><p style={{ color: 'red' }}>Error: {error}</p></div>
  }

  
  return <>

    <h1>Trade Table</h1>
    {trades.length === 0 ? (
      <p>No trades found</p>
    ) : (
      <table className="table table-striped">
        <thead>
          <tr>
            <th>Symbol</th>
            <th>Entry Price</th>
            <th>Quantiy</th>
            <th>Direction</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {trades.map((trade, index) => (
            <tr key={index}>
              <td>{trade.symbol}</td>
              <td>{trade.entryPrice}</td>
              <td>{trade.quantity}</td>
              <td>{trade.direction}</td>
              <td>{trade.status}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )}

  </>
}
