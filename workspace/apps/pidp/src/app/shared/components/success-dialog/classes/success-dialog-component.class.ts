import { DialogBcproviderCreateComponent } from '../components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from '../components/dialog-bcprovider-edit.component';
import { DialogExternalAccountCreateComponent } from '../components/external-account-create.component';
import { FeedbackSendComponent } from '../components/feedback-send.component';

export type SuccessDialogComponentClass =
  | DialogBcproviderCreateComponent
  | DialogBcproviderEditComponent
  | FeedbackSendComponent
  | DialogExternalAccountCreateComponent;
