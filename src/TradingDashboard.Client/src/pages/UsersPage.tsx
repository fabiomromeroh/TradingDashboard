import { Card, CardContent } from "@/components/ui/card";
import { UserTable } from "@/features/users";

const UsersPage = () => {
  return (
    <Card className="flex h-full flex-col overflow-hidden">
      <CardContent className="flex h-full flex-1 flex-col overflow-hidden">
        <UserTable />
      </CardContent>
    </Card>
  );
};

export default UsersPage;
