import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldGroup, FieldLabel, FieldSet } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import { useLoginMutation } from "../hooks/useLoginMutation";
import { useState } from "react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AlertCircleIcon } from "lucide-react";

export default function LoginForm() {
  const navigate = useNavigate();
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const {
    mutate: loginUser,
    isPending: loading,
    error: errors,
  } = useLoginMutation();

  const handleLoginUser = async () => {
    const loginUserCommand = {
      email: email,
      password: password,
    };

    const success = await loginUser(loginUserCommand);

    if (success) {
      navigate("/dashboard");
    }
  };

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
                  onChange={(e) => setEmail(e.target.value)}
                  value={email}
                />
              </Field>
              <Field>
                <FieldLabel htmlFor="password">Password</FieldLabel>

                <Input
                  required
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  onChange={(e) => setPassword(e.target.value)}
                  value={password}
                />
              </Field>
              {errors.length > 0 && (
                <Alert variant="destructive" className="max-w-md">
                  <AlertCircleIcon />
                  <AlertTitle>Login failed</AlertTitle>
                  <AlertDescription>
                    <ul className="list-disc list-inside space-y-1">
                      {errors.map((error, index) => (
                        <li key={index}>{error.message}</li>
                      ))}
                    </ul>
                  </AlertDescription>
                </Alert>
              )}
              <Field>
                <Button
                  onClick={handleLoginUser}
                  disabled={loading}
                  type="submit"
                >
                  Login
                </Button>
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
                    // onClick={() => navigate("/reset-password")}
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
