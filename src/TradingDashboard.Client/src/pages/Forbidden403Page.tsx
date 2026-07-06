import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { ShieldAlert, Home, LogIn } from "lucide-react";

const Forbidden403Page: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-[radial-gradient(circle_at_20%_20%,#131722_0%,#0b0e14_60%,#05060a_100%)] px-6">
      <Card className="w-full max-w-md border border-white/10 bg-slate-900/70 backdrop-blur-md shadow-2xl">
        <CardContent className="flex flex-col items-center text-center p-10">
          <div className="mb-4 flex h-24 w-24 items-center justify-center rounded-full bg-red-500/10">
            <ShieldAlert className="h-12 w-12 text-red-500" strokeWidth={1.5} />
          </div>

          <h1 className="mb-1 text-6xl font-extrabold tracking-tight bg-gradient-to-br from-red-400 to-red-500 bg-clip-text text-transparent">
            403
          </h1>
          <h2 className="mb-3 text-xl font-semibold text-slate-100">
            Access Denied
          </h2>
          <p className="mb-6 text-sm leading-relaxed text-slate-400">
            You don&apos;t have permission to view this section of the{" "}
            <span className="font-semibold text-blue-400">
              Trading Dashboard
            </span>
            . This area is restricted to authorized users only.
          </p>

          <div className="mb-6 h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent" />

          <div className="flex w-full flex-col gap-2.5">
            <Button
              className="w-full bg-gradient-to-r from-blue-500 to-blue-600 shadow-lg shadow-blue-500/30 hover:from-blue-600 hover:to-blue-700"
              onClick={() => navigate("/")}
            >
              <Home className="mr-2 h-4 w-4" />
              Back to Dashboard
            </Button>
            <Button
              variant="outline"
              className="w-full border-white/10 bg-transparent text-slate-300 hover:bg-white/5 hover:text-slate-100"
              onClick={() => navigate("/login")}
            >
              <LogIn className="mr-2 h-4 w-4" />
              Sign in with different account
            </Button>
          </div>

          <p className="mt-6 text-xs text-slate-500">
            Need access? Contact your administrator or{" "}
            <a
              href="mailto:support@tradingdashboard.com"
              className="text-blue-400 hover:underline"
            >
              support@tradingdashboard.com
            </a>
          </p>
        </CardContent>
      </Card>
    </div>
  );
};

export default Forbidden403Page;
