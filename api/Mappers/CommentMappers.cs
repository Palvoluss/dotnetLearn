using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                createdOn = commentModel.createdOn,
                StockId = commentModel.StockId
            };


        }

        public static Comment ToComment(this CreateCommentDto createCommentDto, int stockId)
        {
            return new Comment
            {
                Title = createCommentDto.Title,
                Content = createCommentDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequest updateCommentRequest)
        {
            return new Comment
            {
                Title = updateCommentRequest.Title,
                Content = updateCommentRequest.Content
            };
        }
    }
}