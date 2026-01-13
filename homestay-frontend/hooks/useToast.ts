import { toast } from "sonner";

export const useToast = () => {
  return {
    toast: (props: {
      title?: string;
      description?: string;
      variant?: "default" | "destructive";
    }) => {
      if (props.variant === "destructive") {
        toast.error(props.title || props.description || "Error");
      } else {
        toast.success(props.title || props.description || "Success");
      }
    },
  };
};
