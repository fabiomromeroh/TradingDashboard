import { Card, CardContent } from "@/components/ui/card";
import { UserTable } from "@/features/users";

const UsersPage = () => {
  return (
    <Card>
      <CardContent>
        <UserTable />
      </CardContent>
    </Card>
  );
};

export default UsersPage;
