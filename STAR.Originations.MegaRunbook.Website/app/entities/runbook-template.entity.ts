
import { RunbookStep } from './runbook-step.entity';

export class RunbookTemplate {
    ID: Number;
    Number: Number;
    Name: String;
    Steps: RunbookStep[];
}
