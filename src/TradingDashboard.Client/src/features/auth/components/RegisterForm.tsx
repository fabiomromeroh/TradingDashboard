import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field } from "@/components/ui/field";
// import {
//   Field,
//   FieldDescription,
//   FieldGroup,
//   FieldLabel,
//   FieldSet,
// } from "@/components/ui/field";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useCreateUser } from "@/features/users/hooks/useCreateUser";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import z from "zod";

const userSchema = z.object({
  firstName: z.string().min(1, "First name is required"),
  lastName: z.string().min(1, "Last name is required"),
  email: z.string().email("Enter a valid email address"),
  confirmEmail: z.string().email("Enter a valid email address"),
  password: z.string().min(6, "Password must be at least 8 characters"),
});
type CreateUserFormValues = z.infer<typeof userSchema>;

export default function RegisterForm() {
  const { execute: createUser, error } = useCreateUser();
  const form = useForm<CreateUserFormValues>({
    resolver: zodResolver(userSchema),
    defaultValues: { firstName: "", lastName: "", email: "", password: "" },
  });

  const navigate = useNavigate();

  async function handleFormSubmit(values: CreateUserFormValues) {
    const success = await createUser(values);

    if (success) {
      toast.success("User created successfully");
      form.reset();
    } else {
      toast.error(error ?? "Failed to create user");
    }
  }

  return (
    <Form {...form}>
      <Card>
        <CardContent>
          <CardHeader className="mb-4">
            <CardTitle>Register</CardTitle>
          </CardHeader>
          <form
            onSubmit={form.handleSubmit(handleFormSubmit)}
            className="space-y-4"
          >
            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="firstName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>First name</FormLabel>
                    <FormControl>
                      <Input placeholder="Jane" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="lastName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Last name</FormLabel>
                    <FormControl>
                      <Input placeholder="Doe" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input
                      type="email"
                      placeholder="jane@example.com"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="confirmEmail"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Confirm Email</FormLabel>
                  <FormControl>
                    <Input
                      type="email"
                      placeholder="jane@example.com"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <Input type="password" placeholder="********" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Field>
              <Button type="submit">Register</Button>
            </Field>
            <Field>
              <Button variant="link" onClick={() => navigate("/login")}>
                Login
              </Button>
            </Field>
          </form>
        </CardContent>
      </Card>
    </Form>
  );
}
