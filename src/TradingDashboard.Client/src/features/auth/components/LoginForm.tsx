import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldGroup, FieldLabel, FieldSet } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";

export default function LoginForm() {
  const navigate = useNavigate();

  return (
    <div className="w-full max-w-md">
      <Card>
        <CardContent>
          <CardHeader>
            <CardTitle>Login</CardTitle>
          </CardHeader>
          <FieldSet className="w-full ">
            <FieldGroup>
              <Field>
                <FieldLabel htmlFor="username">Email</FieldLabel>
                <Input
                  required
                  id="username"
                  type="text"
                  placeholder="Max Leiter"
                />
              </Field>
              <Field>
                <FieldLabel htmlFor="password">Password</FieldLabel>

                <Input
                  required
                  id="password"
                  type="password"
                  placeholder="••••••••"
                />
              </Field>
              <Field>
                <Button type="submit">Login</Button>
              </Field>
              <div className="flex flex-col-2 items-center mt-4">
                <Field>
                  <Button variant="link" onClick={() => navigate("/register")}>
                    Register
                  </Button>
                </Field>
                <Field>
                  <Button
                    className="text-gray-500 hover:text-gray-7"
                    variant="link"
                    onClick={() => navigate("/reset-password")}
                  >
                    Forgot Password?
                  </Button>
                </Field>
              </div>
            </FieldGroup>
          </FieldSet>
        </CardContent>
      </Card>
    </div>
  );
}
