import { DialogBcproviderCreateComponent } from '../components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from '../components/dialog-bcprovider-edit.component';
import { FeedbackSendComponent } from '../components/feedback-send.component';

export type SuccessDialogComponentClass =
  | DialogBcproviderCreateComponent
  | DialogBcproviderEditComponent
  | FeedbackSendComponent;
