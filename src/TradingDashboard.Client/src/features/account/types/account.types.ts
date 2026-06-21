
export type AccountStatus = 'active' | 'inactive' | 'pending';

export interface Account {
  id: string;
  name: string;
  brokerId: string;
   brokerName: string;
   userId: string;
  currency: string;

}