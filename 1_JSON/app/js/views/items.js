ResumeViewer.Views.Items = Backbone.View.extend({
  template: _.template($('#tpl-items').html()),

  renderOne: function(item) {
    var itemView = new ResumeViewer.Views.Item({model: item});
    this.$('.items-container').append(itemView.render().$el);
  },

  render: function() {
    var html = this.template();
    this.$el.html(html);

    this.collection.each(this.renderOne, this);

    return this;
  }
}); 