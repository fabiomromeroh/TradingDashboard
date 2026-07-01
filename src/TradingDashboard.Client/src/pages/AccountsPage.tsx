import { Card, CardContent } from "@/components/ui/card";
import { AccountTable } from "@/features/account/components/AccountTable";

export default function AccountsPage() {
  return (
    <Card>
      <CardContent>
        <AccountTable />
      </CardContent>
    </Card>
  );
}
