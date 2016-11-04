
import { Contact }         from './contact.entity';
import { Team }            from './team.entity';
import { RunbookTemplate } from './runbook-template.entity';

export class RFC {
    Number: number;
    Name: string;
    Contacts: Contact;
    Templates: RunbookTemplate[];
}
