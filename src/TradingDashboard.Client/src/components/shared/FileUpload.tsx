import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "../ui/button";
import { cn } from "@/lib/utils";
import { Spinner } from "../ui/spinner";

interface FileUploadProps {
  handleUpload: (file: File) => void;
  className?: string;
  disabled?: boolean;
  isUploading: boolean;
}

export function FileUpload({
  handleUpload,
  className,
  disabled,
  isUploading,
}: FileUploadProps) {
  const [fileName, setFileName] = useState("");

  return (
    <div className={cn("grid w-full items-center gap-4", className)}>
      <Label htmlFor="file">Upload file</Label>
      <Input
        id="file"
        type="file"
        onChange={(e) => {
          const file = e.target.files?.[0];
          setFileName(file ? file.name : "");
        }}
      />
      {fileName ? (
        <p className="text-sm text-muted-foreground">Selected: {fileName}</p>
      ) : null}
      <Button
        disabled={!fileName || disabled || isUploading}
        className="mt-2"
        onClick={() => {
          const fileInput = document.getElementById("file") as HTMLInputElement;
          const file = fileInput.files?.[0];
          if (file) {
            handleUpload(file);
          }
        }}
      >
        {isUploading && <Spinner />}
        Import
      </Button>
    </div>
  );
}
