import { FormGroup } from '@angular/forms';

interface ValidationParams {
  field: string;
  type: string;
  message: string;
}

export function getValidationMessage(params: ValidationParams[], form: FormGroup): string | null {
  for (const param of params) {
    const isInvalid = form.get(param.field)?.hasError(param.type);
    if (isInvalid) return param.message;
  }
  return null;
}
