using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class AttachmentFileRepository : Repository<AttachmentFile>, IAttachmentFileRepository
    {
        public AttachmentFileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
